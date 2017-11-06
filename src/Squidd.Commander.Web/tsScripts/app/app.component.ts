import { Component } from "@angular/core";
@Component({
    selector: "all-runners",
    template: `    
<h4>{{location}}</h4>
<div class="row">
    <div class="col-md-6">
        <div class="card text-center runner-card">
            <div class="card-header">
                {{runner.friendlyName}}
            </div>
            <div class="card-block">
                <i class="fa fa-globe"></i>
                <h5 class="card-title">{{runner.computerName}}</h5>
                <ul class="card-text list-unstyled">
                    <li>{{runner.os}}</li>
                    <li>{{runner.ip}}</li>
                </ul>
            </div>
            <div class="card-footer text-muted">
                Last contact: 2 days ago
            </div>
        </div>
    </div>
</div>
  `
})
export class AppComponent {
    location = "UK South";
    runner: Runner = {
        id: 1,
        friendlyName: "Primary Web Server",
        computerName: "WebServer01",
        ip: "12.34.456.78:13000",
        os: "Windows Server 2012 R2"
    }
}

export class Runner {
    id: number;
    friendlyName: string;
    computerName: string;
    os: string;
    ip: string;
}