import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import {
  TeamResponseDto,
  CreateTeamDto,
  UpdateTeamDto,
  AssignTeamMemberDto
} from '../models/team.model';

@Injectable({ providedIn: 'root' })
export class TeamService {

  constructor(private apiService: ApiService) {}

  // ============================================================
  // GET: api/Teams
  // ============================================================
  getAll(): Observable<TeamResponseDto[]> {
    return this.apiService
      .send<any>('GET', 'Teams')
      .pipe(
        map((res: any) => this.normalizeArray<TeamResponseDto>(res)),
        tap(teams => console.log(`Fetched ${teams.length} teams`)),
        catchError(error => {
          console.error('Failed to load teams:', error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // GET: api/Teams/{id}
  // ============================================================
  getById(id: number): Observable<TeamResponseDto> {
    return this.apiService
      .send<TeamResponseDto>('GET', `Teams/${id}`)
      .pipe(
        catchError(error => {
          console.error(`Failed to load team ${id}:`, error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // POST: api/Teams
  // ============================================================
  create(dto: CreateTeamDto): Observable<TeamResponseDto> {
    return this.apiService
      .send<TeamResponseDto>('POST', 'Teams', dto)
      .pipe(
        tap(team => console.log('Team created:', team)),
        catchError(error => {
          console.error('Failed to create team:', error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // PUT: api/Teams/{id}
  // ============================================================
  update(id: number, dto: UpdateTeamDto): Observable<TeamResponseDto> {
    return this.apiService
      .send<TeamResponseDto>('PUT', `Teams/${id}`, dto)
      .pipe(
        tap(team => console.log('Team updated:', team)),
        catchError(error => {
          console.error(`Failed to update team ${id}:`, error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // POST: api/Teams/members
  // ============================================================
  assignMember(dto: AssignTeamMemberDto): Observable<{ message: string }> {
    return this.apiService
      .send<{ message: string }>('POST', 'Teams/members', dto)
      .pipe(
        tap(res => console.log('Member assigned:', res)),
        catchError(error => {
          console.error('Failed to assign member:', error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // DELETE: api/Teams/{teamId}/members/{userId}
  // ============================================================
  removeMember(teamId: number, userId: number): Observable<void> {
    return this.apiService
      .send<void>('DELETE', `Teams/${teamId}/members/${userId}`)
      .pipe(
        tap(() => console.log(`Removed user ${userId} from team ${teamId}`)),
        catchError(error => {
          console.error(`Failed to remove member from team ${teamId}:`, error);
          return throwError(() => error);
        })
      );
  }

  // ============================================================
  // Normalize response (handles $values wrapper from .NET)
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