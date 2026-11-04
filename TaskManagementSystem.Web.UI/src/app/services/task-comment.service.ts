import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import {
  TaskCommentResponseDto,
  CreateTaskCommentDto
} from '../models/task-comment.model';

@Injectable({ providedIn: 'root' })
export class TaskCommentService {

  constructor(private apiService: ApiService) {}

  // ============================================================
  // GET: api/tasks/{taskId}/comments
  // ============================================================
  getByTaskId(taskId: number): Observable<TaskCommentResponseDto[]> {
    return this.apiService
      .send<any>('GET', `tasks/${taskId}/comments`)
      .pipe(
        map((res: any) => this.normalizeArray<TaskCommentResponseDto>(res)),
        tap(comments => console.log(`Fetched ${comments.length} comments for task ${taskId}`)),
        catchError(error => {
          console.error(`Failed to load comments for task ${taskId}:`, error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // POST: api/tasks/{taskId}/comments
  // ============================================================
  addComment(
    taskId: number,
    comment: string
  ): Observable<TaskCommentResponseDto> {
    const dto: CreateTaskCommentDto = { taskId, comment };

    return this.apiService
      .send<TaskCommentResponseDto>('POST', `tasks/${taskId}/comments`, dto)
      .pipe(
        tap((c) => console.log('Comment added:', c)),
        catchError(error => {
          console.error('Failed to add comment:', error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // Normalize response (handles $values from .NET)
  // ============================================================
  private normalizeArray<T>(res: any): T[] {
    if (!res) return [];
    if (Array.isArray(res)) return res as T[];
    if (Array.isArray(res.$values)) return res.$values as T[];
    if (typeof res === 'object') {
      const vals = Object.values(res).filter(
        (v) => v && typeof v === 'object' && 'id' in (v as any)
      ) as T[];
      if (vals.length > 0) return vals;
    }
    console.warn('Unexpected collection response:', res);
    return [];
  }
}