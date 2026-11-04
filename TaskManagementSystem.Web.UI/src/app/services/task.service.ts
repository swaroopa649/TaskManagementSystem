import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import { AssignTaskDto, CreateTaskDto, TaskResponseDto, TaskStatusResponseDto, UpdateTaskDto } from '../models/task.model';


@Injectable({ providedIn: 'root' })
export class TaskService {

  constructor(private apiService: ApiService) {}

  // ============================================================
  // GET: api/Tasks
  // ============================================================
  getAll(): Observable<TaskResponseDto[]> {
    return this.apiService
      .send<any>('GET', 'Tasks')
      .pipe(
        map((res: any) => this.normalizeArray<TaskResponseDto>(res)),
        tap(tasks => console.log(`Fetched ${tasks.length} tasks`)),
        catchError(error => {
          console.error('Failed to load tasks:', error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // GET: api/Tasks/my
  // ============================================================
  getMyTasks(): Observable<TaskResponseDto[]> {
    return this.apiService
      .send<any>('GET', 'Tasks/my')
      .pipe(
        map((res: any) => this.normalizeArray<TaskResponseDto>(res)),
        catchError(error => {
          console.error('Failed to load my tasks:', error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // GET: api/Tasks/{id}
  // ============================================================
  getById(id: number): Observable<TaskResponseDto> {
    return this.apiService
      .send<TaskResponseDto>('GET', `Tasks/${id}`)
      .pipe(
        catchError(error => {
          console.error(`Failed to load task ${id}:`, error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // POST: api/Tasks
  // ============================================================
  create(dto: CreateTaskDto): Observable<TaskResponseDto> {
    return this.apiService
      .send<TaskResponseDto>('POST', 'Tasks', dto)
      .pipe(
        tap(t => console.log('Task created:', t)),
        catchError(error => {
          console.error('Failed to create task:', error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // PUT: api/Tasks/{id}
  // ============================================================
  update(id: number, dto: UpdateTaskDto): Observable<TaskResponseDto> {
    return this.apiService
      .send<TaskResponseDto>('PUT', `Tasks/${id}`, dto)
      .pipe(
        tap(t => console.log('Task updated:', t)),
        catchError(error => {
          console.error(`Failed to update task ${id}:`, error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // POST: api/Tasks/assign
  // ============================================================
  assign(dto: AssignTaskDto): Observable<TaskResponseDto> {
    return this.apiService
      .send<TaskResponseDto>('POST', 'Tasks/assign', dto)
      .pipe(
        tap(t => console.log('Task assigned:', t)),
        catchError(error => {
          console.error('Failed to assign task:', error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // PATCH: api/Tasks/{id}/status?statusId=X
  // ============================================================
  updateStatus(id: number, statusId: number): Observable<void> {
    return this.apiService
      .send<void>('PATCH', `Tasks/${id}/status?statusId=${statusId}`, {})
      .pipe(
        tap(() => console.log(`Task ${id} status → ${statusId}`)),
        catchError(error => {
          console.error(`Failed to update status for task ${id}:`, error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // DELETE: api/Tasks/{id}
  // ============================================================
  delete(id: number): Observable<void> {
    return this.apiService
      .send<void>('DELETE', `Tasks/${id}`)
      .pipe(
        tap(() => console.log(`Task ${id} deleted`)),
        catchError(error => {
          console.error(`Failed to delete task ${id}:`, error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // GET: api/TaskStatus
  // ============================================================
  getStatuses(): Observable<TaskStatusResponseDto[]> {
    return this.apiService
      .send<any>('GET', 'TaskStatus')
      .pipe(
        map((res: any) => this.normalizeArray<TaskStatusResponseDto>(res)),
        catchError(error => {
          console.error('Failed to load task statuses:', error);
          return throwError(() => error);
        })
      );
  }

  private normalizeArray<T>(res: any): T[] {
    if (!res) return [];
    if (Array.isArray(res)) return res as T[];
    if (Array.isArray(res.$values)) return res.$values as T[];
    return [];
  }
}