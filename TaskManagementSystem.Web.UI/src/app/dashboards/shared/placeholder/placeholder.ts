import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-placeholder',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="page-header">
      <div class="page-block">
        <div class="row align-items-center">
          <div class="col-md-12">
            <div class="page-header-title">
              <h5 class="m-b-10">Coming Soon</h5>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="row">
      <div class="col-md-8 mx-auto">
        <div class="card">
          <div class="card-body text-center py-5">
            <i class="feather icon-tool f-40 text-muted"></i>
            <h5 class="mt-3">This page is under construction</h5>
            <p class="text-muted">
              The feature will be available soon.
            </p>
            <a routerLink="/" class="btn btn-primary mt-2">
              <i class="feather icon-home"></i> Go Home
            </a>
          </div>
        </div>
      </div>
    </div>
  `
})
export class PlaceholderComponent {}