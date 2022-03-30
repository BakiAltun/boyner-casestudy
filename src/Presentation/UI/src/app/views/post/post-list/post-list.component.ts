import { Component, OnInit } from '@angular/core';
import { Post, PostService } from '../shared/post.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ["./post-list.component.css"]
})
export class PostListComponent implements OnInit {

  data: Post[] = [];
  constructor(private postService: PostService) {

  }

  ngOnInit(): void {
    this.getData(1, 10);
  }

  totalItems: any = new Array(0);
  getData(page: number, pageSize: number) {
    this.data = [];

    this.postService
      .getAll(page, pageSize)
      .subscribe((res) => {


        this.totalItems = new Array(Math.round(res.totalItems / 10));
        this.data = res.items;
      });
  }


  trackByFn(_: any, item: Post) {
    return item.id;
  }

}
