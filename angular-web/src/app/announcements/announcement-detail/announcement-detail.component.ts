import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { AnnouncementService } from '@proxy/announcements';
import { AnnouncementDetailDto } from '@proxy/announcements';

@Component({
  selector: 'app-announcement-detail',
  templateUrl: './announcement-detail.component.html',
  styleUrls: ['./announcement-detail.component.scss'],
  imports: [CommonModule, NgbAlertModule],
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
      this.error = '公告ID不存在';
    }
  }

  loadAnnouncement(id: string) {
    this.loading = true;
    this.error = null;

    this.announcementService
      .get(id)
      .subscribe({
        next: (result: AnnouncementDetailDto) => {
          this.announcement = result;
          this.loading = false;
        },
        error: (err) => {
          console.error('加载公告失败', err);
          this.error = '公告不存在或已被删除';
          this.loading = false;
        }
      });
  }

  goBack() {
    this.router.navigate(['/announcements']);
  }
}
