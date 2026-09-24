import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders,
  HttpRequest,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { filter, map, catchError, tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from '../../environment';
import { AuthResponse, ApplicationUser } from '../models/auth.response';

type BodylessMethod = 'GET' | 'HEAD' | 'DELETE' | 'OPTIONS';
type BodyMethod = 'POST' | 'PUT' | 'PATCH';
type HttpMethod = BodylessMethod | BodyMethod;

// Circuit Breaker States
export enum CircuitState {
  CLOSED = 'CLOSED',
  OPEN = 'OPEN',
  HALF_OPEN = 'HALF_OPEN'
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  // Circuit Breaker properties
  private circuitState: CircuitState = CircuitState.CLOSED;
  private failureCount = 0;
  private lastFailureTime = 0;
  private halfOpenAttempts = 0;

  private readonly FAILURE_THRESHOLD = 3;
  private readonly RESET_TIMEOUT = 30000;
  private readonly HALF_OPEN_MAX_ATTEMPTS = 1;

  // Expose circuit state for UI feedback
  public circuitState$ = new BehaviorSubject<CircuitState>(CircuitState.CLOSED);

  constructor(private http: HttpClient, private router: Router) { }

  //------------------------------------
  // Circuit Breaker Methods
  //------------------------------------
  private canExecute(): boolean {
    if (this.circuitState === CircuitState.CLOSED) {
      return true;
    }

    if (this.circuitState === CircuitState.OPEN) {
      const now = Date.now();
      if (now - this.lastFailureTime >= this.RESET_TIMEOUT) {
        this.circuitState = CircuitState.HALF_OPEN;
        this.halfOpenAttempts = 0;
        this.circuitState$.next(CircuitState.HALF_OPEN);
        console.log('Circuit Breaker: Moving to HALF_OPEN state');
        return true;
      }
      console.warn(
        `Circuit Breaker: OPEN. Retry after ${(this.RESET_TIMEOUT - (now - this.lastFailureTime)) / 1000}s`
      );
      return false;
    }

    if (this.circuitState === CircuitState.HALF_OPEN) {
      if (this.halfOpenAttempts >= this.HALF_OPEN_MAX_ATTEMPTS) {
        console.warn('Circuit Breaker: HALF_OPEN - max attempts reached');
        return false;
      }
      this.halfOpenAttempts++;
      return true;
    }

    return true;
  }

  private onSuccess(): void {
    if (this.circuitState === CircuitState.HALF_OPEN) {
      this.circuitState = CircuitState.CLOSED;
      this.failureCount = 0;
      this.halfOpenAttempts = 0;
      this.circuitState$.next(CircuitState.CLOSED);
      console.log('Circuit Breaker: CLOSED (success in half-open)');
    } else {
      this.failureCount = 0;
    }
  }

  private onFailure(): void {
    this.failureCount++;
    this.lastFailureTime = Date.now();

    if (this.circuitState === CircuitState.HALF_OPEN) {
      this.circuitState = CircuitState.OPEN;
      this.circuitState$.next(CircuitState.OPEN);
      console.log('Circuit Breaker: OPEN (failure in half-open)');
      return;
    }

    if (this.failureCount >= this.FAILURE_THRESHOLD) {
      this.circuitState = CircuitState.OPEN;
      this.circuitState$.next(CircuitState.OPEN);
      console.log(`Circuit Breaker: OPEN (${this.failureCount} failures)`);
    }
  }

  //------------------------------------
  // Main Request Method
  //------------------------------------
  send<TResponse>(method: BodylessMethod, url: string): Observable<TResponse>;
  send<TResponse>(method: BodyMethod, url: string, body: any): Observable<TResponse>;
  send<TResponse>(method: HttpMethod, url: string, body?: any): Observable<TResponse> {
    if (!this.canExecute()) {
      return throwError(() => new Error('Circuit Breaker is OPEN. Service temporarily unavailable.'));
    }

    const headers = this.getDefaultHeaders();
    const fullUrl = this.buildFullUrl(url);
    const request = this.buildRequest(method, fullUrl, body, headers);

    return this.execute<TResponse>(request);
  }

  //------------------------------------
  // Private Helper Methods
  //------------------------------------
  private getDefaultHeaders(): HttpHeaders {
    const token = this.getJwtToken();
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }

    return headers;
  }

  private buildFullUrl(endpoint: string): string {
    const base = environment.baseUrl.replace(/\/+$/, '');
    const path = endpoint.replace(/^\/+/, '');
    return `${base}/${path}`;
  }

  private buildRequest(
    method: HttpMethod,
    url: string,
    body: any,
    headers: HttpHeaders
  ): HttpRequest<any> {
    if (method === 'GET' || method === 'HEAD' || method === 'DELETE' || method === 'OPTIONS') {
      return new HttpRequest(method, url, { headers });
    }
    return new HttpRequest(method, url, JSON.stringify(body), { headers });
  }

  private execute<T>(request: HttpRequest<any>): Observable<T> {
    return this.http.request<T>(request).pipe(
      filter(event => event instanceof HttpResponse),
      map((event: any) => {
        const response = event as HttpResponse<T>;
        return this.handleResponse<T>(response);
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 || error.status === 402) {
          this.handleAuthFailure();
        }
        this.onFailure();
        return throwError(() => error);
      }),
      tap({
        next: () => this.onSuccess()
      })
    );
  }

  private handleResponse<T>(response: HttpResponse<T>): T {
    if (response.status >= 200 && response.status < 300) {
      if (response.body === null && response.status === 204) {
        return true as unknown as T;
      }
      return response.body as T;
    }

    console.error('Error response:', response);
    throw new Error(`HTTP error: ${response.status} - ${response.statusText}`);
  }

  private handleAuthFailure(): void {
    localStorage.removeItem('AuthenticatedUserResponse');
    console.warn('Authentication failed. Please login again.');
    this.router.navigate(['/login']);
  }

  //------------------------------------
  // Storage Helper Methods (private)
  //------------------------------------
  private getAuthData(): AuthResponse | null {
    const raw = localStorage.getItem('AuthenticatedUserResponse');
    return raw ? (JSON.parse(raw) as AuthResponse) : null;
  }

  private getJwtToken(): string {
    const authData = this.getAuthData();
    return authData?.jwtToken ?? '';
  }

  //------------------------------------
  // Public Methods
  //------------------------------------
  setAuthData(authResponse: AuthResponse): void {
    localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(authResponse));
  }

  clearAuthData(): void {
    localStorage.removeItem('AuthenticatedUserResponse');
  }

  getCurrentUser(): ApplicationUser | null {
    return this.getAuthData()?.applicationUser ?? null;
  }

  isAuthenticated(): boolean {
    const authData = this.getAuthData();
    return authData !== null && !!authData.jwtToken;
  }

  /** Public accessor for the JWT — used by tokenInterceptor and guards. */
  getToken(): string {
    return this.getJwtToken();
  }
}