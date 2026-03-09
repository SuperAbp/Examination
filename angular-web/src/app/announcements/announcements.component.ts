import { Component, OnInit, inject } from '@angular/core';

import { RouterLink } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AnnouncementService } from '@proxy/controllers';
import { CoreModule, ListResultDto } from '@abp/ng.core';
import { AnnouncementListDto } from '@proxy/announcements';

@Component({
  selector: 'app-announcements',
  templateUrl: './announcements.component.html',
  styleUrls: ['./announcements.component.scss'],
  imports: [CoreModule, NgbAlertModule, RouterLink],
  standalone: true,
})
export class AnnouncementsComponent implements OnInit {
  private announcementService = inject(AnnouncementService);

  announcements: AnnouncementListDto[] = [];
  loading = false;

  ngOnInit() {
    this.loadAnnouncements();
  }

  loadAnnouncements() {
    this.loading = true;

    this.announcementService.getList().subscribe((result: ListResultDto<AnnouncementListDto>) => {
      this.announcements = result.items || [];
      this.loading = false;
    });
  }
}
