import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostDetailComponent } from './post-detail/post-detail.component';
import { PostListComponent } from './post-list/post-list.component';
import { PostFormComponent } from './shared/post-form/post-form.component';
import { PostRoutingModule } from './post-routing.module';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  imports: [
    CommonModule,
    PostRoutingModule,
    ReactiveFormsModule
  ],
  declarations: [
    PostDetailComponent,
    PostListComponent,
    PostFormComponent
  ], 
})
export class PostModule { }
