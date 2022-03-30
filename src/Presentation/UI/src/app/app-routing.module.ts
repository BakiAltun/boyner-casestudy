import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [ 
  {
    path: '', redirectTo: "/post", pathMatch: "full"
  },
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
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
