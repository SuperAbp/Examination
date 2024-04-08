import { NgModule } from '@angular/core';
import { STWidgetRegistry } from '@delon/abc/st';
import { NzTabsModule } from 'ng-zorro-antd/tabs';

import { SharedModule } from '../shared.module';
import { STSetPriceWidget } from './st-set-price.widget';
export const STWIDGET_COMPONENTS = [STSetPriceWidget];

@NgModule({
  declarations: STWIDGET_COMPONENTS,
  imports: [SharedModule, NzTabsModule],
  exports: [...STWIDGET_COMPONENTS]
})
export class STWidgetModule {
  constructor(widgetRegistry: STWidgetRegistry) {
    widgetRegistry.register(STSetPriceWidget.KEY, STSetPriceWidget);
  }
}
