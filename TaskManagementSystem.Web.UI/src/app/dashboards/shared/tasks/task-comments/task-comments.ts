import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { TaskCommentResponseDto } from '../../../../models/task-comment.model';
import { TaskCommentService } from '../../../../services/task-comment.service';
import { AccountService } from '../../../../services/account.service';


@Component({
  selector: 'app-task-comments',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-comments.html',
  styleUrls: ['./task-comments.css']
})
export class TaskCommentsComponent implements OnInit, OnDestroy {

  @Input() taskId!: number;

  comments: TaskCommentResponseDto[] = [];
  newComment = '';

  isLoading = false;
  isPosting = false;

  private sub?: Subscription;
  private postSub?: Subscription;

  constructor(
    private commentService: TaskCommentService,
    public account: AccountService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    if (!this.taskId) {
      console.error('TaskCommentsComponent requires a [taskId] input');
      return;
    }
    this.loadComments();
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
    this.postSub?.unsubscribe();
  }

  // ============================================================
  // LOAD
  // ============================================================
  loadComments(): void {
    this.isLoading = true;

    this.sub?.unsubscribe();
    this.sub = this.commentService.getByTaskId(this.taskId).subscribe({
      next: (data) => {
        this.comments = Array.isArray(data) ? data : [];
        this.isLoading = false;
      },
      error: () => {
        this.comments = [];
        this.isLoading = false;
        this.toastr.error('Failed to load comments.', 'Error');
      }
    });
  }

  // ============================================================
  // POST
  // ============================================================
  postComment(): void {
    const text = this.newComment.trim();
    if (!text) {
      this.toastr.warning('Please enter a comment.', 'Validation');
      return;
    }

    if (this.isPosting) return;

    this.isPosting = true;

    this.postSub?.unsubscribe();
    this.postSub = this.commentService.addComment(this.taskId, text).subscribe({
      next: (created) => {
        // Append to local list
        this.comments = [...this.comments, created];
        this.newComment = '';
        this.isPosting = false;
        this.toastr.success('Comment added.', 'Success');
      },
      error: (err) => {
        this.isPosting = false;
        this.toastr.error(
          err?.error?.message || 'Failed to add comment.',
          'Error'
        );
      }
    });
  }

  // ============================================================
  // HELPERS
  // ============================================================
  initials(name: string): string {
    const parts = (name ?? '').trim().split(/\s+/);
    const f = parts[0]?.charAt(0) ?? '';
    const l = parts[1]?.charAt(0) ?? '';
    return (f + l).toUpperCase() || '?';
  }

  isMine(comment: TaskCommentResponseDto): boolean {
    return comment.userId === this.account.getUserId();
  }

  trackByCommentId(index: number, c: TaskCommentResponseDto): number {
    return c.id;
  }
}