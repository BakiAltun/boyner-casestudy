import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { PostService, Post } from '../shared/post.service';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html'
})
export class PostDetailComponent implements OnInit {
  id: number = 0;
  form = new FormGroup({
    text: new FormControl(''),
  });
  constructor(
    private postService: PostService,
    private route: ActivatedRoute,
    private router: Router) {

  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.id = params['id'];
    });

    // this.id = this.route.snapshot.paramMap.get('id');
  }
  onSubmit() {

    var model: Post = {
      id: this.id,
      text: this.form.get("text")?.value
    };

    this.postService.savePost(model)
      .subscribe((res) => {
        this.router.navigate([''], { relativeTo: this.route });
      });

  }
}
