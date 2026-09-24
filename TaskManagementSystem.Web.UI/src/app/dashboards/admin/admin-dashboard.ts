import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-dashboard.html',
  styleUrls: ['./admin-dashboard.css']
})
export class AdminDashboardComponent {
  constructor(public account: AccountService) {}
}