import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() usersHome: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor() { }

  ngOnInit(): void {
  }

  register() {
    
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
