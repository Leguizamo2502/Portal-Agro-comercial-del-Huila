import { Component, Input } from '@angular/core';
import { SkeletonComponent } from "../skeleton/skeleton.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-skeleton-lines',
  imports: [SkeletonComponent,CommonModule],
  templateUrl: './skeleton-lines.component.html',
  styleUrl: './skeleton-lines.component.css',
})
export class SkeletonLinesComponent {
  @Input() lines = 3;
  @Input() lineWidth = '100%';
  @Input() lineHeight = '14px';
}
