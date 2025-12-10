import { RoutesService, eLayoutType } from '@abp/ng.core';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';
import { inject, provideAppInitializer } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  console.log(routes);

  routes.removeByParam({ name: eThemeSharedRouteNames.Administration });
  routes.add([
    {
      path: '/',
      name: '::Menu:Home',
      iconClass: 'fas fa-home',
      order: 1,
      layout: eLayoutType.application,
    },
  ]);
  routes.add([
    {
      path: '/exams',
      name: '::OnlineExam',
      iconClass: 'fas fa-home',
      order: 1,
      layout: eLayoutType.application,
    },
  ]);
  routes.add([
    {
      path: '/question-banks',
      name: '::QuestionBanks',
      iconClass: 'fas fa-home',
      order: 1,
      layout: eLayoutType.application,
    },
  ]);
}
