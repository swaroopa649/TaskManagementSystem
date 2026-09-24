import { Component } from '@angular/core';
import { SidebarComponent } from './sidebar/sidebar';
import { HeaderComponent } from './header/header';
import { BodyComponent } from './body/body';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [SidebarComponent, HeaderComponent, BodyComponent],
  templateUrl: './layout.html',
  styleUrls: ['./layout.css']
})
export class LayoutComponent {}