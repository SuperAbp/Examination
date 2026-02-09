import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AnnouncementService } from '@proxy/announcements';
import { CoreModule, ListResultDto } from '@abp/ng.core';
import { AnnouncementListDto } from '@proxy/announcements';

@Component({
  selector: 'app-announcements',
  templateUrl: './announcements.component.html',
  styleUrls: ['./announcements.component.scss'],
  imports: [
    CoreModule,
    CommonModule,
    NgbAlertModule,
    RouterLink
  ],
  standalone: true,
})
export class AnnouncementsComponent implements OnInit {
  private announcementService = inject(AnnouncementService);

  announcements: AnnouncementListDto[] = [];
  loading = false;
  error: string | null = null;

  ngOnInit() {
    this.loadAnnouncements();
  }

  loadAnnouncements() {
    this.loading = true;
    this.error = null;
    
    this.announcementService
      .getEffectiveList()
      .subscribe({
        next: (result: ListResultDto<AnnouncementListDto>) => {
          this.announcements = result.items || [];
          this.loading = false;
        },
        error: (err) => {
          console.error('加载公告失败', err);
          this.error = '加载公告失败，请稍后重试';
          this.loading = false;
        }
      });
  }
}
