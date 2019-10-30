import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MediaUploadService } from 'src/app/services/media/media-upload.service';
import { AdminService } from 'src/app/services/admin/admin.service';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';

@Component({
  selector: "app-season",
  templateUrl: "./season.component.html"
})
export class SeasonComponent implements OnInit {

  type: string;
  form: any;
  progress: number;
  message: string;
  uploading: boolean;
  list: Array<TitleView>;
  searchObject: TitleView;
  pageNumber: number;

  constructor(
    private readonly fb: FormBuilder,
    private readonly media: MediaUploadService,
    private readonly admin: AdminService,
    private readonly snack: MatSnackBar,
    private readonly router: Router
  ) {}

  ngOnInit() {
    this.list = new Array<TitleView>();
    this.pageNumber = 1;
    this.uploading = false;
    this.getTitlePage();
    this.form = this.fb.group({
      name: '',
      description: '',
      titleId: [2],
      imageId: ''
    });
  }

  getTitle(name: string) {
    this.admin.getTitleViewByName(name).subscribe(value => {
      this.searchObject = value.body;
    });
  }

  getTitlePage() {
    this.admin.getTitlesPage(this.pageNumber, 5).subscribe(value => {
      this.list.push(value.body);
      this.pageNumber++;
      console.log(this.list)
    });
  }
}

export interface TitleView {
  id: number;
  name: string;
}
