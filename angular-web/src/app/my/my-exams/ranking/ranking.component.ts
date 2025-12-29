import { CoreModule, ConfigStateService } from '@abp/ng.core';
import { Component, inject, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { ExaminationService } from '@proxy/controllers';
import { ExamRankingDto } from '@proxy/exam-management/exams';

@Component({
  selector: 'app-exam-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.scss'],
  imports: [CoreModule, CommonModule, NgxDatatableModule],
})
export class ExamRankingComponent {
  private readonly examinationService = inject(ExaminationService);
  private readonly configState = inject(ConfigStateService);
  private readonly modalService = inject(NgbModal);

  @ViewChild('rankingModal', { static: true }) rankingModal: TemplateRef<any>;

  rankingList: ExamRankingDto[] = [];
  isLoadingRanking = false;
  currentUserId: string = '';
  private modalRef: NgbModalRef | null = null;

  open(examId: string) {
    this.currentUserId = this.configState.getOne('currentUser')?.id || '';
    this.loadRanking(examId);
    this.modalRef = this.modalService.open(this.rankingModal, {
      size: 'lg',
      centered: true,
    });
  }

  loadRanking(examId: string) {
    this.isLoadingRanking = true;
    this.rankingList = [];

    this.examinationService.getRankingList(examId).subscribe({
      next: response => {
        this.rankingList = response.items;
        this.isLoadingRanking = false;
      },
      error: () => {
        this.isLoadingRanking = false;
      },
    });
  }

  close() {
    if (this.modalRef) {
      this.modalRef.close();
      this.modalRef = null;
    }
  }

  getRowClass(row: ExamRankingDto) {
    return {
      'table-info': row.userId === this.currentUserId,
    };
  }
}
