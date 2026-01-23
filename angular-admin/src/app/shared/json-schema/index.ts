import type { SFWidgetProvideConfig } from '@delon/form';

import { TestWidget } from './test/test.widget';
import { withTreeSelectWidget } from '@delon/form/widgets/tree-select';

export const SF_WIDGETS: SFWidgetProvideConfig[] = [{ KEY: TestWidget.KEY, type: TestWidget }, withTreeSelectWidget()];
