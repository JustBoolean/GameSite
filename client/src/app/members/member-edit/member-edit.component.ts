import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { faUpload } from '@fortawesome/free-solid-svg-icons';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  member: Member;
  user: User;
  uploadIcon = faUpload;

  uploader: FileUploader;
  hasBaseDropZoneOver: false;
  baseUrl = environment.apiUrl;
  fileNew: File = null;


  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService, private router: Router) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user
    });
  }

  ngOnInit(): void {
    this.loadMember();
    this.initializeUploader();
  }

  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(member => {
      this.member = member;
    })
  }

  updateMember() {
    this.memberService.updateMember(this.member).subscribe(() => {
      this.toastr.success('Profile Saved');
      this.editForm.reset(this.member);
      if(this.fileNew != null) {
        this.memberService.setPhoto(this.fileNew);
      }
    }); 
    this.reloadCurrentRoute();
  }

  initializeUploader() {
    this.uploader = new FileUploader( {
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: true,
      maxFileSize: 10*1024*1024 //cloudinary max size 10mb
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
      this.fileNew = file._file;
    }
  }
  fileOverBase(event: any) {
    this.hasBaseDropZoneOver = event;
  }

  reloadCurrentRoute() {
    setTimeout(() => {},30000);
    let currentUrl = this.router.url;
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
        this.router.navigate([currentUrl]);
        console.log(currentUrl);
    });
  }

}
