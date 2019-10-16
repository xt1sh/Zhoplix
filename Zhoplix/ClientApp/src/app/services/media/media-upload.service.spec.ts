import { TestBed } from '@angular/core/testing';

import { MediaUploadService } from './media-upload.service';

describe('MediaUploadService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MediaUploadService = TestBed.get(MediaUploadService);
    expect(service).toBeTruthy();
  });
});
