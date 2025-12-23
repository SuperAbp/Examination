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
  routes.removeByParam({ name: eThemeSharedRouteNames.Administration });
  routes.add([
    {
      path: '/',
      name: '::Menu:Home',
      order: 1,
      layout: eLayoutType.application,
    },
  ]);
  routes.add([
    {
      path: '/exams',
      name: '::Menu:OnlineExam',
      order: 2,
      layout: eLayoutType.application,
    },
  ]);
  routes.add([
    {
      path: '/question-banks',
      name: '::Menu:QuestionBank',
      order: 3,
      layout: eLayoutType.application,
    },
  ]);
  routes.add([
    {
      name: '::Menu:My',
      order: 4,
      layout: eLayoutType.application,
    },
    {
      path: '/my/exams',
      name: '::Menu:MyExam',
      parentName: '::Menu:My',
      order: 1,
      layout: eLayoutType.application,
    },
    {
      path: '/my/favorites',
      name: '::Menu:MyFavorite',
      parentName: '::Menu:My',
      order: 1,
      layout: eLayoutType.application,
    },
    {
      path: '/my/mistakes-reviews',
      name: '::Menu:MyMistakeReview',
      parentName: '::Menu:My',
      order: 1,
      layout: eLayoutType.application,
    },
  ]);
}
