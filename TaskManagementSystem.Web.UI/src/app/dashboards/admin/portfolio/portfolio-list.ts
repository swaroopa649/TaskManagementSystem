import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { PortfolioService } from '../../../services/portfolio.service';
import { PortfolioResponseDto } from '../../../models/portfoilio.model';
import { CommonModule } from '@angular/common';
import { SpinnerLoadingService } from '../../../services/spinner-loading-service';

@Component({
  imports: [CommonModule],
  selector: 'app-portfolio-list',
  styleUrl: './portfolio-list.css',
  templateUrl: './portfolio-list.html',
})
export class PortfolioListComponent implements OnInit {
  isLoading = false;

  portfolios: PortfolioResponseDto[] = [];

  constructor(
    private portfolioService: PortfolioService,
    private spinner: SpinnerLoadingService,
    private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.spinner.show('Loading portfolios...');
    this.portfolioService.getPortfoliosAsync().subscribe({
      next: (data) => {
        this.portfolios = data;
        this.isLoading = false;
        this.spinner.hide();
         this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error fetching portfolios:', error);
        this.isLoading = false;
        this.spinner.hide();
        this.cdr.detectChanges();
      }
    });
  }

  editPortfolio(portfolioId: number): void {
    // Implement the logic to edit the portfolio with the given ID
    console.log(`Edit portfolio with ID: ${portfolioId}`);
  }
}
