import { Component ,Inject, OnInit} from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
 
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit  {

  LevenshteinModel: LevenshteinModel[] = [];
  private baseUrl: string;
  private http: HttpClient;
  diffstring: string;
  JwtToken: string;
  personForm: FormGroup;
  errors: string[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseURL: string, private fb: FormBuilder) {
    this.http = http;
    this.baseUrl = baseURL;
    this.diffstring = "";
    this.JwtToken = "";

    this.http.get('https://localhost:44356/api/Login?username=arun&password=arun') // kept username and password as static
      .subscribe(result => { this.JwtToken=result["token"]; }, error => console.error(error));
  }

  ngOnInit() {
    this.personForm = this.fb.group({
      FirstParam: ['', Validators.required],
      SecondParam: ['', Validators.required]       
    });
    this.errors = [];
  }

  
  onSubmit(FirstParam, SecondParam) {
    if (this.JwtToken == "") { alert("Authentication Failed"); return false; }
    if (this.personForm.valid) {
      this.diffstring = "";
      let headers = new HttpHeaders({
        'Authorization': "Bearer " + this.JwtToken,
        'Accept': 'application/json',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Credentials': 'true',
        'Access-Control-Allow-Methods': 'POST,GET,DELETE,PUT',
        'Access-Control-Allow-Headers': 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With',
        'Access-Control-Expose-Headers': 'responseType'
      });

      let options = { headers: headers };
      var levenParams = { "FirstParam": this.personForm.value.FirstParam, "SecondParam": this.personForm.value.SecondParam };
      this.http.post('https://localhost:44356/api/Login/GetValue', levenParams, options)
        .subscribe(result => { this.diffstring = JSON.stringify(result); console.log(result); }, error => console.error(error));
    } else {
      this.errors.push("something went wrong!");
    }
  }
}

interface LevenshteinModel{
  FirstParam:string,
  SecondParam:string,
  DifferenceCount: string;
}
