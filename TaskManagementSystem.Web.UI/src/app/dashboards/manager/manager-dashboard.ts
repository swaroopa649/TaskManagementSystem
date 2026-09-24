import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './manager-dashboard.html',
  styleUrls: ['./manager-dashboard.css']
})
export class ManagerDashboardComponent implements OnInit {

  constructor(public account: AccountService) {}

  ngOnInit(): void {
    // Hook for loading manager-specific data later
  }
}