import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {

  email = '';
  password = '';
  rememberMe = false;

  constructor(private router: Router) {}

  signin(): void {
    // TODO: replace with real API call
    if (this.email && this.password) {
      // pretend login succeeded
      localStorage.setItem('isLoggedIn', 'true');
      this.router.navigate(['/dashboard']);
    } else {
      alert('Please enter email and password');
    }
  }
}