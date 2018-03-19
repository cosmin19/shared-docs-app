import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/components/common/messageservice';

@Injectable()
export class AlertService {

    constructor(private messageService: MessageService) {}

    success(summary: string, detail: string) {
        this.messageService.add({severity:'success', summary: summary, detail: detail});
    }

    error(summary: string, detail: string) {
        this.messageService.add({severity:'error', summary: summary, detail: detail});
    }

    info(summary: string, detail: string) {
        this.messageService.add({severity:'info', summary: summary, detail: detail});
    }

    warn(summary: string, detail: string) {
        this.messageService.add({severity:'warn', summary: summary, detail: detail});
    }

    clear() {
        this.messageService.clear();
    }
}