import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { faUser, faComment, faHeart } from '@fortawesome/free-solid-svg-icons';
import { MembersService } from 'src/app/_services/members.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member : Member;

  userIcon = faUser;
  heartIcon = faHeart;
  messageIcon = faComment;
  constructor(private memberService: MembersService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  addFollow(member: Member) {
    this.memberService.addFollow(member.userName).subscribe(() => {
      this.toastr.success('You are following ' + member.knownAs);
    })
  }

}
