import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/account.service';


@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-dashboard.html',
  styleUrls: ['./user-dashboard.css']
})
export class UserDashboardComponent implements OnInit {

  constructor(public account: AccountService) {}

  ngOnInit(): void {
    // Hook for loading user-specific data later
  }
}