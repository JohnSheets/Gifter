import { useState } from "react"
import { useNavigate } from "react-router-dom"
import { addPost } from "../APIManagers/PostManager.js";


export const PostForm = () => {
    const navigate = useNavigate()
    const [newPost, update] = useState({
        title: "",
        imageUrl: "",
        caption: "",
        userProfileId: 1, 
        dateCreated: Date.now()
    })

    const handleSaveButtonClick = (event) => {
        event.preventDefault()
        const postToSendToAPI = {
            Title: newPost.title,
            Caption: newPost.caption,
            ImageUrl: newPost.imageUrl,
            UserProfileId: 1,
            DateCreated: new Date().toISOString()
        }
        return addPost(postToSendToAPI)
            .then(navigate("/"));
    }
    return(
        <div>
       <form className="post-form">
        <h2 className="post-form-title">Create New Post</h2>
        <fieldset>
            <div className="form-grop">
                <label htmlFor="title"> Title:</label>
                <input 
                    type="text"
                    id="title"
                    value={newPost.title}
                    onChange={
                        (event) => {
                            const copy = {...newPost}
                            copy.title = event.target.value
                            update(copy)
                        }
                    } />
            </div>
        </fieldset>
        <fieldset>
            <div className="form-grop">
                <label htmlFor="caption"> Caption:</label>
                <input 
                    type="text"
                    id="caption"
                    value={newPost.caption}
                    onChange={
                        (event) => {
                            const copy = {...newPost}
                            copy.caption = event.target.value
                            update(copy)
                        }
                    } />
            </div>
        </fieldset>
        <fieldset>
            <div className="form-grop">
                <label htmlFor="imageUrl"> ImageUrl:</label>
                <input 
                    type="text"
                    id="imageUrl"
                    value={newPost.imageUrl}
                    onChange={
                        (event) => {
                            const copy = {...newPost}
                            copy.imageUrl = event.target.value
                            update(copy)
                        }
                    } />
            </div>
        </fieldset>
        <button
            onClick={(clickEvent) => handleSaveButtonClick(clickEvent)} className="btn-primary">Submit Post</button>
        </form></div>
    ) 
}