import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
@Injectable({
    providedIn: 'root',
})
export class PostService {

    constructor(private http: HttpClient) { }
    getById(id: string) {
        return this.http.get<any>(environment.apiUrl + "Post/" + id).pipe(
            retry(3), // retry a failed request up to 3 times
            catchError(this.handleError) // then handle the error
        );
    }
    getAll(page: number, pageSize: number) {
        return this.http.get<any>(environment.apiUrl + 'Post?page=' + page + "&pageSize=" + pageSize).pipe(
            retry(3), // retry a failed request up to 3 times
            catchError(this.handleError) // then handle the error
        );
    }

    save(post: Post) {
debugger
        if (post.id && post.id.length> 0  && post.id !== "0") return this.updatePost(post);

        return this.addPost(post);
    }

    addPost(post: Post): Observable<Post> {
        return this.http.post<Post>(environment.apiUrl + "Post", post, httpOptions)
            .pipe(
                catchError(() => this.handleError2('addPost', post))
            );
    }

    updatePost(post: Post): Observable<Post> {
        return this.http.put<Post>(environment.apiUrl + "Post/" + post.id, post, httpOptions)
            .pipe(
                catchError(() => this.handleError2('addPost', post))
            );
    }

    private handleError(error: HttpErrorResponse) {
        if (error.status === 0) {
            console.error('An error occurred:', error.error);
        } else {
            console.error(`Backend returned code ${error.status}, body was: `, error.error);
        }
        return throwError(() => new Error('Something bad happened; please try again later.'));
    }

    private handleError2(key: string, model: Post) {

        console.error(`Backend returned error for ${key}, model was: `, model);

        return throwError(() => new Error('Something bad happened; please try again later.'));
    }
}

const httpOptions = {
    headers: new HttpHeaders({
        // 'Content-Type': 'application/json',
        // Authorization: 'my-auth-token'
    })
};
export interface Post {
    id: string |null,
    text?: string,
    createdOn?: Date,
    updatedOn?: Date
}