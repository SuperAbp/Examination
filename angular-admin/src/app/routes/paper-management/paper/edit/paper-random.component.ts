import { CoreModule, LocalizationService } from '@abp/ng.core';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { PageHeaderModule } from '@delon/abc/page-header';
import { ModalHelper } from '@delon/theme';
import { dateTimePickerUtil } from '@delon/util';
import {
  GetPaperForEditorOutput,
  PaperCreateOrUpdateDtoBase_PaperSectionDto,
  PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionRuleDto
} from '@proxy/admin/paper-management/papers';
import { QuestionBankListDto } from '@proxy/admin/question-management/question-banks';
import { KnowledgePointNodeDto } from '@proxy/admin/knowledge-points';
import { KnowledgePointService, OptionService, PaperService, QuestionBankService, QuestionService } from '@proxy/admin/controllers';
import { SharedModule, simplifiedOrdinary } from '@shared';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzFlexModule } from 'ng-zorro-antd/flex';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';
import { finalize, tap } from 'rxjs/operators';

import { QuestionRandomComponent } from './question-random.component';
import { QuestionSelectComponent } from './question-select.component';
import { RuleRandomComponent } from './rule-random.component';

export interface PaperQuestionRuleCreateTemp extends PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionRuleDto {
  id?: string;
  questionBankName: string;
  questionTypeName: string;
  knowledgePointId?: string;
}
@Component({
  selector: 'app-paper-random',
  templateUrl: './paper-random.component.html',
  styles: [
    `
      .ant-form-item-label {
        width: 95px;
      }
      .box {
        border: 1px solid #ddd;
        padding: 10px;
        margin: 15px 0;
        border-radius: 4px;
      }
    `
  ],
  imports: [
    SharedModule,
    CoreModule,
    PageHeaderModule,
    FooterToolbarModule,
    NzSpinModule,
    NzCardModule,
    NzFormModule,
    NzIconModule,
    NzInputModule,
    NzListModule,
    NzInputNumberModule,
    NzButtonModule,
    NzFlexModule,
    NzSpaceModule,
    NzModalModule,
    NzRadioModule,
    NzDescriptionsModule,
    NzSelectModule,
    NzTableModule,
    NzPopconfirmModule,
    NzTooltipModule
  ]
})
export class PaperManagementPaperRandomEditComponent implements OnInit {
  private modal = inject(ModalHelper);
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private paperService = inject(PaperService);
  private questionService = inject(QuestionService);
  private questionBankService = inject(QuestionBankService);
  private knowledgePointService = inject(KnowledgePointService);
  private optionService = inject(OptionService);
  private messageService = inject(NzMessageService);
  private localizationService = inject(LocalizationService);

  paperId: string;
  paper: GetPaperForEditorOutput;

  loading = false;
  isConfirmLoading = false;
  showPaperTime: boolean;
  form: FormGroup = null;

  // Modal related
  currentSectionIndex = -1;
  questionBanks: QuestionBankListDto[] = [];
  knowledgePoints: KnowledgePointNodeDto[] = [];

  get totalScore() {
    return this.sections.controls.reduce((total, section) => {
      const questionsArray = section.get('paperQuestionRules') as FormArray;
      const sectionTotalScore = questionsArray.controls.reduce(
        (sum, q) => sum + (q.get('score')?.value || 0) * (q.get('count')?.value || 0),
        0
      );
      return total + sectionTotalScore;
    }, 0);
  }
  get sections() {
    return this.form?.get('sections') as FormArray;
  }

  getRules(sectionIndex: number): FormArray {
    const section = this.sections.at(sectionIndex);
    return section.get('paperQuestionRules') as FormArray;
  }

  getTotalQuestionsCount(sectionIndex: number): number {
    const rulesArray = this.getRules(sectionIndex);
    return rulesArray.controls.reduce((sum, rule) => sum + (rule.get('count')?.value || 0), 0);
  }

  getQuestionBankName(questionBankId: string): string {
    return this.questionBanks.find(b => b.id === questionBankId)?.title || '';
  }

  getKnowledgePointName(knowledgePointId: string): string {
    if (!knowledgePointId) return '-';
    // Use recursive search or flatten list if knowledgePoints is tree
    // KnowledgePointNodeDto has children.
    // For simplicity, let's assume we can flatten it or search recursively.
    const findName = (nodes: KnowledgePointNodeDto[], id: string): string | undefined => {
      for (const node of nodes) {
        if (node.id === id) return node.name;
        if (node.children) {
          const found = findName(node.children, id);
          if (found) return found;
        }
      }
      return undefined;
    };
    return findName(this.knowledgePoints, knowledgePointId) || knowledgePointId;
  }

  ngOnInit(): void {
    this.loading = true;
    this.loadQuestionBanksAndTypes();
    this.route.paramMap.subscribe(params => {
      let id = params.get('id');
      this.paperId = id;
      if (this.paperId) {
        this.paperService
          .getEditor(this.paperId)
          .pipe(
            tap(response => {
              this.paper = response;
              this.buildForm();
              this.loading = false;
            })
          )
          .subscribe();
      } else {
        this.paper = {} as GetPaperForEditorOutput;
        this.buildForm();
        this.loading = false;
      }
    });
  }

  loadQuestionBanksAndTypes() {
    this.questionBankService
      .getList({ skipCount: 0, maxResultCount: 100 })
      .pipe(
        tap(res => {
          this.questionBanks = res.items;
        })
      )
      .subscribe();

    this.knowledgePointService.getAll({}).subscribe(res => {
      this.knowledgePoints = res.items;
    });
  }

  buildForm() {
    this.form = this.fb.group({
      name: [this.paper.name || '', [Validators.required]],
      description: [this.paper.description || ''],
      paperType: [1],
      sections: this.fb.array((this.paper.sections || []).map(s => this.createSection(s)))
    });

    // Initialize rule scores and compute totals
    this.onRuleChange();
  }

  createSection(section?: PaperCreateOrUpdateDtoBase_PaperSectionDto) {
    return this.fb.group({
      id: [section?.id || ''],
      title: [section?.title || '', [Validators.required]],
      scoreEach: [section?.scoreEach, [Validators.required, Validators.min(1)]],
      order: [section?.order || 0],
      remark: [section?.remark || ''],
      paperQuestionRules: this.fb.array((section?.paperQuestionRules || []).map(r => this.createRule(r)))
    });
  }

  createRule(rule?: PaperQuestionRuleCreateTemp | PaperCreateOrUpdateDtoBase_PaperSectionDto_PaperQuestionRuleDto) {
    return this.fb.group({
      id: [rule?.id || null],
      questionBankId: [rule?.questionBankId || null, [Validators.required]],
      questionType: [rule?.questionType !== null && rule?.questionType !== undefined ? rule.questionType : null, [Validators.required]],
      count: [rule?.count || 0, [Validators.required, Validators.min(1)]],
      score: [rule?.score || 0, [Validators.required, Validators.min(1)]],
      knowledgePointId: [rule?.knowledgePointId || null]
    });
  }

  addSection() {
    this.sections.push(
      this.createSection({
        title: `第${simplifiedOrdinary(this.sections.length + 1)}大题`,
        scoreEach: 0
      } as PaperCreateOrUpdateDtoBase_PaperSectionDto)
    );
  }

  openRuleModal(sectionIndex: number) {
    this.currentSectionIndex = sectionIndex;
    const section = this.sections.at(sectionIndex);
    const rulesArray = section.get('paperQuestionRules') as FormArray;
    const scoreEach = section.get('scoreEach')?.value || 0;

    // Prepare selected rules from FormArray for the modal
    const selectedRules = rulesArray.controls.map(control => ({
      questionBankId: control.get('questionBankId').value,
      questionType: control.get('questionType').value,
      count: control.get('count').value,
      knowledgePointId: control.get('knowledgePointId')?.value
    }));

    this.modal.createStatic(RuleRandomComponent, { selectedRules }, { size: 'lg' }).subscribe(result => {
      if (result) {
        // Check for duplicates and merge count if found
        const duplicateIndex = rulesArray.controls.findIndex(
          control =>
            control.get('questionBankId').value === result.questionBankId &&
            control.get('questionType').value === result.questionType &&
            control.get('knowledgePointId')?.value === result.knowledgePointId
        );

        if (duplicateIndex > -1) {
          // Merge count with existing rule
          const existingControl = rulesArray.at(duplicateIndex);
          const existingCount = existingControl.get('count').value || 0;
          existingControl.get('count').setValue(existingCount + result.count);
        } else {
          const rule: PaperQuestionRuleCreateTemp = {
            id: null,
            questionBankId: result.questionBankId,
            questionType: result.questionType,
            count: result.count,
            score: scoreEach,
            questionBankName: '',
            questionTypeName: '',
            knowledgePointId: result.knowledgePointId
          };

          rulesArray.push(this.createRule(rule));
        }

        this.recomputeTotal();
      }
    });
  }

  deleteRule(sectionIndex: number, ruleIndex: number) {
    const section = this.sections.at(sectionIndex);
    const rulesArray = section.get('paperQuestionRules') as FormArray;
    rulesArray.removeAt(ruleIndex);

    this.recomputeTotal();
  }

  trashSection(index) {
    const sectionsArray = this.sections;
    sectionsArray.removeAt(index);
    this.recomputeTotal();
  }

  onRuleChange() {
    // Update rule scores based on scoreEach
    this.sections.controls.forEach(sectionControl => {
      const scoreEach = sectionControl.get('scoreEach')?.value || 0;
      const rulesArray = sectionControl.get('paperQuestionRules') as FormArray;
      rulesArray.controls.forEach(rule => {
        rule.get('score')?.setValue(scoreEach);
      });
    });

    this.recomputeTotal();
  }

  recomputeTotal() {
    if (!this.form) return;
    let totalScore = 0;

    this.sections.controls.forEach(sectionControl => {
      const rulesArray = sectionControl.get('paperQuestionRules') as FormArray;
      const scoreEach = sectionControl.get('scoreEach')?.value || 0;
      const sectionTotalScore = rulesArray.controls.reduce((sum, r) => sum + (r.get('count')?.value || 0) * scoreEach, 0);
      const sectionTotalCount = rulesArray.controls.reduce((sum, r) => sum + (r.get('count')?.value || 0), 0);

      sectionControl.get('totalScore')?.setValue(sectionTotalScore);
      sectionControl.get('totalCount')?.setValue(sectionTotalCount);

      totalScore += sectionTotalScore;
    });
  }

  private assignOrderValues(formValue: any) {
    formValue.sections.forEach((section, sectionIndex) => {
      section.order = sectionIndex;
      section.paperQuestionRules.forEach((rule, ruleIndex) => {
        rule.order = ruleIndex;
      });
    });
    return formValue;
  }

  save() {
    if (!this.form.valid || this.isConfirmLoading) {
      for (const key of Object.keys(this.form.controls)) {
        this.form.controls[key].markAsDirty();
        this.form.controls[key].updateValueAndValidity();
      }
      return;
    }
    this.isConfirmLoading = true;

    const formValue = this.assignOrderValues({ ...this.form.value });

    if (this.paperId) {
      this.paperService
        .update(this.paperId, {
          ...this.paper,
          ...formValue
        })
        .pipe(
          tap(() => {
            this.goback();
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    } else {
      this.paperService
        .create(formValue)
        .pipe(
          tap(() => {
            this.goback();
          }),
          finalize(() => (this.isConfirmLoading = false))
        )
        .subscribe();
    }
  }

  back(e: MouseEvent) {
    e.preventDefault();
    this.goback();
  }
  goback() {
    this.router.navigateByUrl('/paper-management/paper');
  }
}
