import { Component, AfterViewInit } from '@angular/core';

declare var ApexCharts: any; // if apexcharts is loaded globally

@Component({
  selector: 'app-body',
  standalone: true,
  imports: [],
  templateUrl: './body.html',
  styleUrls: ['./body.css']
})
export class BodyComponent implements AfterViewInit {

  ngAfterViewInit(): void {
    // Re-init any charts/plugins that depend on the DOM
    // e.g. call your dashboard-main.js init functions here
  }
}