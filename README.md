# Alten Booking
This project was made to solve a code challange as follows...

## CHALLENGE
Post-Covid scenario:
People are now free to travel everywhere but because of the pandemic, a lot of hotels
went bankrupt. Some former famous travel places are left with only one hotel.
You've been given the responsibility to develop a booking API for the very last hotel in Cancun.
The requirements are:
- API will be maintained by the hotel's IT department.
- As it's the very last hotel, the quality of service must be 99.99 to 100% => no downtime
- For the purpose of the test, we assume the hotel has only one room available
- To give a chance to everyone to book the room, the stay can't be longer than 3 days
and can't be reserved more than 30 days in advance.
- All reservations start at least the next day of booking,
- To simplify the use case, a 'DAY' in the hotel room starts from 00:00 to 23:59:59.
- Every end-user can check the room availability, place a reservation, cancel it or modify it.
- To simplify the API is insecure.

## PROPOSED APPROACH
In order to achieve no downtime, some techniques could be implemented such as:
- Blue Green Deployment: the redundant environment can serve as a fallback in the occurance of a crisis;
- Kubernetes Cluster: the application could be containerized and deployed to a kubernetes cluster, in order to enjoy all the resilience features. The kubernetes cluster could be hosted in the cloud as a managed service with region redundancy;

## PROJECT ARCHITECTURE AND PATTERNS
- The solution was structured using "Ports and Adapters" architectural pattern;
- As much as possible, domain classes were designed not to be anemic;
- Application layer makes use of UnitOfWork and Repository patterns that are implemented at the Infrastructure layer using Entity Framework Core and SQLite;
- One cool thing about the Repository, is that it is generic and supports the Query Object Pattern. Queries are considered business logic and, because of that, remains at the Domain layer;
- For unit testing, a fake Repository was implemented using Moq library and List<T> collection;
- Browsing the Git history, it's possible to notice that TDD was used to develop the project. All tests were written first. =]
- At the API layer, a middleware is responsible for handling all exceptions;