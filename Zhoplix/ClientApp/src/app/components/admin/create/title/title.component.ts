import { AdminService } from './../../../../services/admin/admin.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MediaUploadService } from 'src/app/services/media/media-upload.service';

@Component({
  selector: 'app-title',
  templateUrl: './title.component.html',
})
export class TitleComponent implements OnInit {

  type: string;
  form: any;
  progress: number;
  message: string;

  constructor(private readonly fb: FormBuilder,
              private readonly media: MediaUploadService,
              private readonly admin: AdminService) { }

  ngOnInit() {
    this.form = this.fb.group({
      name: '',
      description: '',
      ageRestriction: '0',
      imageId: null
    })
    this.media.getProgress().subscribe(value => {
      this.progress = value;
    });
  }

  uploadPhoto(files) {
    this.media.uploadPhoto(files[0]);
    this.media.getMessage().subscribe(value => {
      this.form.controls['imageId'].setValue(value);
    })
  }

  onSubmit() {
    this.admin.createTitle(this.form.value).subscribe(() => {
      this.message = 'Все четко'
    }, () => {
      this.message = 'Пошел нахуй?'
    });
  }
}
