import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostDetailComponent } from './views/post/post-detail/post-detail.component';

const routes: Routes = [
  {
    path: '',
    // component: AuthComponent
    children: [
      {
        path: 'post',
        loadChildren: () => import('./views/post/post.module').then((m) => m.PostModule),
        data: { title: "Post" }
      }
    ]
  },
  // {
  //   path: '', redirectTo: "/dashboard", pathMatch: "full"
  // }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
