import { useState } from "react"
import { searchPosts } from "../APIManagers/PostManager"
import { Button } from "reactstrap"

export const PostSearch = () => {
    const [searchQuery, setSearchQuery] = useState("")
    const [searchResults, setSearchResults] = useState([])

    const handleSearchInputChange = (event) => {
        setSearchQuery(event.target.value);
    }

    const handleSearchButtonClick = (event) => {
        event.preventDefault();
    
        searchPosts(searchQuery)
          .then((response) => {
            setSearchResults(response);
          })
      }

    return(<>
    
    <div >
        <form className="post-form">
          <label htmlFor="searchQuery">Search:</label>
          <input
            type="text"
            id="searchQuery"
            value={searchQuery}
            onChange={handleSearchInputChange}
          />
        </form>

        <Button onClick={handleSearchButtonClick} color="primary">
          Search
        </Button>
      </div>
      {searchResults.length > 0 && (
        <div>
          <h3>Search Results:</h3>
          <div>
            {searchResults.map((post) => (
              <div key={post.id}>
                <h4>{post.title}</h4>
                <p>Caption: {post.caption}</p>
                <img src={post.imageUrl} alt={post.title} />
              </div>
            ))}
          </div>
        </div>
      )}

    </>)
}