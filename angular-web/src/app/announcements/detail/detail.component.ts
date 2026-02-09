import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AnnouncementService } from '@proxy/announcements';
import { AnnouncementDetailDto } from '@proxy/announcements';
import { CoreModule } from '@abp/ng.core';

@Component({
  selector: 'app-announcements-detail',
  templateUrl: './detail.component.html',
  imports: [CoreModule, CommonModule, NgbAlertModule],
  standalone: true,
})
export class AnnouncementDetailComponent implements OnInit {
  private announcementService = inject(AnnouncementService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  announcement: AnnouncementDetailDto | null = null;
  loading = false;
  error: string | null = null;

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadAnnouncement(id);
    } else {
      this.goBack();
    }
  }

  loadAnnouncement(id: string) {
    this.loading = true;
    this.error = null;

    this.announcementService.get(id).subscribe((result: AnnouncementDetailDto) => {
      this.announcement = result;
      this.loading = false;
    });
  }

  goBack() {
    this.router.navigate(['/announcements']);
  }
}
