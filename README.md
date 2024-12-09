# Book Information Aggregator API

A simple REST API for managing and retrieving book information from both local storage and the [Open Library API](https://openlibrary.org/developers/api). The API allows users to:
- Fetch books from local storage.
- Add, delete, or retrieve books locally by ID.
- Search for books or retrieve book details via the Open Library API.

## Features
1. **Local Data Management**:
   - Fetch all locally stored books.
   - Add a new book to local storage.
   - Delete a book by ID.

2. **External API Integration**:
   - Search for books by title or author using the Open Library API.
   - Fetch detailed information about a specific book by ID from the Open Library API.

3. **Postman Collection Included**:
   - A Postman collection is available for testing all the API endpoints.

---

## Endpoints

### 1. **GET /books**
Fetch all locally stored books.

#### Request:
- **Method**: `GET`
- **URL**: `/books`

#### Response:
- **200 OK**:
  ```json
  {
      "message": "Books retrieved successfully.",
      "count": 3,
      "books": [
          {
              "id": "1",
              "title": "To Kill a Mockingbird",
              "author": "Harper Lee",
              "description": "A novel about the serious issues of rape and racial inequality.",
              "published_year": 1960
          },
          ...
      ]
  }
  ```
- **404 Not Found**:
  ```json
  {
      "message": "No books available in the collection."
  }
  ```

---

### 2. **POST /books**
Add a new book to local storage.

#### Request:
- **Method**: `POST`
- **URL**: `/books`
- **Body** (JSON):
  ```json
  {
      "id": "4",
      "title": "The Great Gatsby",
      "author": "F. Scott Fitzgerald",
      "description": "A story of the Jazz Age in the United States.",
      "published_year": 1925
  }
  ```

#### Response:
- **201 Created**:
  ```json
  {
      "message": "Book added successfully.",
      "book": {
          "id": "4",
          "title": "The Great Gatsby",
          "author": "F. Scott Fitzgerald",
          "description": "A story of the Jazz Age in the United States.",
          "published_year": 1925
      }
  }
  ```
- **400 Bad Request**:
  ```json
  {
      "message": "Invalid book data. 'Id', 'Title', and 'Author' fields are required."
  }
  ```
- **409 Conflict**:
  ```json
  {
      "message": "A book with ID '4' already exists."
  }
  ```

---

### 3. **DELETE /books/{id}**
Delete a book by its ID.

#### Request:
- **Method**: `DELETE`
- **URL**: `/books/{id}`

#### Response:
- **200 OK**:
  ```json
  {
      "message": "Book with ID '1' has been successfully deleted."
  }
  ```
- **404 Not Found**:
  ```json
  {
      "message": "Book with ID '1' not found."
  }
  ```

---

### 4. **GET /books/search?query={query}**
Search for books by title or author using the Open Library API.

#### Request:
- **Method**: `GET`
- **URL**: `/books/search?query=The Witcher Blood of Elves`

#### Response:
- **200 OK**:
  ```json
  [
      {
          "key": "OL37501425W",
          "title": "The Witcher. Blood of elves",
          "author": ["Andrzej Sapkowski"],
          "first_publish_year": 2021,
          "first_sentence": ["The town was in flames."],
          "format": ["Paperback"]
      },
      ...
  ]
  ```
- **400 Bad Request**:
  ```json
  {
      "message": "The query field is required."
  }
  ```

---

### 5. **GET /books/{id}**
Fetch detailed information about a book by its Open Library ID.

#### Request:
- **Method**: `GET`
- **URL**: `/books/{id}`

#### Response:
- **200 OK**:
  ```json
  {
      "key": "OL3140822W",
      "title": "To Kill a Mockingbird",
      "first_publish_date": "1969",
      "description": "One of the best-loved stories of all time...",
      "authors": [
          { "author": { "key": "/authors/OL498120A" } }
      ],
      "covers": [12606502],
      "subject_places": ["Southern States", "Maycomb", "Alabama", "Deep South"],
      "subject_people": ["Jean Louise Finch", "Jeremy Finch", "Dill", "Arthur Radley", "Judge Taylor"],
      "subjects": ["fiction", "fiction classics"],
      "subject_times": ["1933-35"],
      "first_sentence": "When he was nearly thirteen, my brother Jem got his arm badly broken at the elbow."
  }
  ```
- **404 Not Found**:
  ```json
  {
      "message": "Book with ID OL3140822W not found."
  }
  ```

---

## How to Run the Application

1. **Navigate to the Solution Directory**:
   - Use the `cd` command to navigate to the folder containing your `.sln` file. For example:
     ```bash
     cd BookInformationAggregatorAPI
     ```

2. **Install Dependencies**:
   - Ensure you have the .NET SDK installed. You can check by running:
     ```bash
     dotnet --version
     ```

3. **Build and Run**:
   ```bash
   dotnet build
   dotnet run
   ```

4. **Test with Postman**:
   - Import the provided Postman collection file (`BookInformationAggregator.postman_collection.json`) to test all the API endpoints.

## Postman Collection

A Postman collection is included for easy testing of the API.

### How to Use:
1. Open Postman.
2. Click **Import** in the top-left corner.
3. Select the `BookInformationAggregator.postman_collection.json` file.
4. Test the available requests.

### Included Requests:
1. **GET /books**: Retrieve all locally stored books.
2. **POST /books**: Add a new book to local storage.
3. **POST /books** (Conflict): Add a book with an already existing ID to test conflict handling.
4. **DELETE /books/{id}**: Delete a specific book by ID.
5. **DELETE /books/{non-existent-id}**: Attempt to delete a non-existent book to test error handling.
6. **GET /books/search?query={query}**: Search for books in the Open Library API using a query string.
7. **GET /books/search?query=**: Search with an empty query to test error handling.
8. **GET /books/{id}**: Fetch details for a specific book by its Open Library ID.
9. **GET /books/{non-existent-id}**: Fetch details for a non-existent book ID to test error handling.

---

## Sample books.json File
```json
[
    {
        "id": "1",
        "title": "To Kill a Mockingbird",
        "author": "Harper Lee",
        "description": "A novel about the serious issues of rape and racial inequality.",
        "published_year": 1960
    },
    {
        "id": "2",
        "title": "1984",
        "author": "George Orwell",
        "description": "A dystopian novel set in a totalitarian society under constant surveillance.",
        "published_year": 1949
    },
    {
        "id": "3",
        "title": "Moby Dick",
        "author": "Herman Melville",
        "description": "The narrative of Captain Ahab's obsessive quest to kill the white whale, Moby Dick.",
        "published_year": 1851
    }
]
```
