import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/shared/models/order';
import { OrdersService } from '../order.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit{
  order?: Order;
  constructor(private orderService: OrdersService, private route: ActivatedRoute){}

  ngOnInit(): void {
    const id = this.route.snapshot.params['id'];
    if(!id) return;
    
    this.orderService.getOrder(id).subscribe({
      next: order => this.order = order
    })
  }
  
}
