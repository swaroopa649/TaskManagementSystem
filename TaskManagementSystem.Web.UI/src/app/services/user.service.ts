import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import {
    UserResponseDto,
    CreateUserDto,
    UpdateUserDto
} from '../models/user.model';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    constructor(private apiService: ApiService) { }

    // ============================================================
    // GET: api/User
    // ============================================================
    getAll(): Observable<UserResponseDto[]> {
        return this.apiService
            .send<UserResponseDto[]>('GET', 'User')
            .pipe(
                tap(users => console.log(`Fetched ${users?.length ?? 0} users`)),
                catchError(error => {
                    console.error('Failed to load users:', error);
                    return throwError(() => error);
                })
            );
    }

    // ============================================================
    // GET: api/User/{id}
    // ============================================================
    getById(id: number): Observable<UserResponseDto> {
        return this.apiService
            .send<UserResponseDto>('GET', `User/${id}`)
            .pipe(
                catchError(error => {
                    console.error(`Failed to load user ${id}:`, error);
                    return throwError(() => error);
                })
            );
    }

    // ============================================================
    // POST: api/User
    // ============================================================
    create(dto: CreateUserDto): Observable<UserResponseDto> {
        return this.apiService
            .send<UserResponseDto>('POST', 'User', dto)
            .pipe(
                tap(user => console.log('User created:', user)),
                catchError(error => {
                    console.error('Failed to create user:', error);
                    return throwError(() => error);
                })
            );
    }

    // ============================================================
    // PUT: api/User/{id}
    // ============================================================
    update(id: number, dto: UpdateUserDto): Observable<UserResponseDto> {
        return this.apiService
            .send<UserResponseDto>('PUT', `User/${id}`, dto)
            .pipe(
                tap(user => console.log('User updated:', user)),
                catchError(error => {
                    console.error(`Failed to update user ${id}:`, error);
                    return throwError(() => error);
                })
            );
    }

    // ============================================================
    // PATCH: api/User/{id}/status?isActive=true|false
    // ============================================================
    updateStatus(id: number, isActive: boolean): Observable<any> {
        return this.apiService
            .send<UserResponseDto>('PUT', `User/${id}`, id)
            .pipe(
                tap(user => console.log('User updated:', user)),
                catchError(error => {
                    console.error(`Failed to update user ${id}:`, error);
                    return throwError(() => error);
                })
            );
    }

    // ============================================================
    // DELETE: api/User/{id}
    // ============================================================
    delete(id: number): Observable<void> {
        return this.apiService
            .send<void>('DELETE', `User/${id}`)
            .pipe(
                tap(() => console.log(`User ${id} deleted`)),
                catchError(error => {
                    console.error(`Failed to delete user ${id}:`, error);
                    return throwError(() => error);
                })
            );
    }
}