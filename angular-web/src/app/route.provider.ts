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
      name: '::Menu:OnlineExam',
      iconClass: 'fas fa-home',
      order: 2,
      layout: eLayoutType.application,
    },
  ]);
  routes.add([
    {
      path: '/question-banks',
      name: '::Menu:QuestionBank',
      iconClass: 'fas fa-home',
      order: 3,
      layout: eLayoutType.application,
    },
  ]);
  routes.add([
    {
      name: '::Menu:My',
      iconClass: 'fas fa-user',
      order: 4,
      layout: eLayoutType.application,
    },
    {
      path: '/my/exams',
      name: '::Menu:MyExam',
      parentName: '::Menu:My',
      iconClass: 'fas fa-star',
      order: 1,
      layout: eLayoutType.application,
    },
    {
      path: '/my/favorites',
      name: '::Menu:MyFavorite',
      parentName: '::Menu:My',
      iconClass: 'fas fa-star',
      order: 1,
      layout: eLayoutType.application,
    },
    {
      path: '/my/mistakes-reviews',
      name: '::Menu:MyMistakeReview',
      parentName: '::Menu:My',
      iconClass: 'fas fa-star',
      order: 1,
      layout: eLayoutType.application,
    },
  ]);
}
