import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import {
  TaskStatusResponseDto,
  CreateTaskStatusDto
} from '../models/task-status.model';

@Injectable({ providedIn: 'root' })
export class TaskStatusService {

  constructor(private apiService: ApiService) {}

  // GET: api/taskstatus  (adjust path when controller exists)
  getAll(): Observable<TaskStatusResponseDto[]> {
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

  // POST: api/taskstatus
  create(dto: CreateTaskStatusDto): Observable<TaskStatusResponseDto> {
    return this.apiService
      .send<TaskStatusResponseDto>('POST', 'TaskStatus', dto)
      .pipe(
        tap(s => console.log('Task status created:', s)),
        catchError(error => {
          console.error('Failed to create task status:', error);
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