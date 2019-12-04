import { AdminService } from './../../../../services/admin/admin.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MediaUploadService } from 'src/app/services/media/media-upload.service';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import * as _ from 'lodash';

@Component({
  selector: 'app-title',
  templateUrl: './title.component.html',
})
export class TitleComponent implements OnInit {

  type: string;
  form: any;
  message: string;
  uploading: boolean;
  progress: number;
  videoPaths: Array<string>;

  constructor(private readonly fb: FormBuilder,
              private readonly media: MediaUploadService,
              private readonly admin: AdminService,
              private readonly snack: MatSnackBar,
              private readonly router: Router) { }

  ngOnInit() {
    this.videoPaths = new Array<string>();
    this.progress = 0;
    this.uploading = false;
    this.form = this.fb.group({
      name: ['', Validators.required],
      description: '',
      genres: this.fb.array([]),
      ageRestriction: 0,
      imageId: '',
      isMovie: false,
      videoPaths: []
    });
    this.addGenre();
  }

  addGenre() {
    this.form.get('genres').push(this.fb.group({name: ''}));
  }

  removeGenre(index: number) {
    this.form.get('genres').removeAt(index);
  }

  uploadPhoto(files) {
    this.uploading = true;
    this.media.uploadPhoto(files[0]);
    this.media.getMessage().subscribe(value => {
      if(value) {
        this.form.controls['imageId'].setValue(value);
        this.uploading = false;
      }
    });
  }

  uploadVideo(files) {
    this.uploading = true;
    this.media.uploadVideo(files[0]);
    this.media.getProgress().subscribe(value => {
      if (value < 99) {
        this.progress = value;
      } else {
        this.progress = 99;
      }
    });
    this.media.getBody().subscribe(value => {
      if (value) {
        this.form.controls['videoPaths'].setValue(value);
        this.progress = 100;
        this.uploading = false;
      }
    }, null, () => {
      this.progress = 0;
    });
  }

  onSubmit() {
    const genres = _.map(this.form.controls['genres'].value, 'name');
    const title = this.form.value;
    title.genres = genres;
    let obs;
    if(this.form.controls['isMovie'].value) {
      obs = this.admin.createMovie(title);
    } else {
      obs = this.admin.createTitle(title);
    }
    obs.subscribe(() => {
      this.message = 'Created';
      this.snack.open(`Title "${this.form.controls['name'].value}" was successfully created`,
        'OK', {duration: 3000, panelClass: ['snack-success']});
      this.router.navigateByUrl('/admin/create');
    }, () => {
      this.message = 'Problems'
    });
  }
}
