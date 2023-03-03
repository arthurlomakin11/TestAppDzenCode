import Styles from "./CommentReplyForm.module.scss";
import React, {useState} from "react";

export let CommentReplyForm = () => {
    const [message, setMessage] = useState('');

    const handleMessageChange = (event: { target: { value: React.SetStateAction<string>; }; }) => {
        setMessage(event.target.value);
    };
    
    function ToolbarElementClick() {
        /*setMessage()*/
    }
    
    return <div className={Styles.CommentReplyForm}>
        <ul>
            <li><button onClick={ToolbarElementClick}>a</button></li>
            <li>i</li>
            <li>strong</li>
            <li>code</li>
        </ul>
        <textarea value={message} onChange={handleMessageChange}/>
    </div>
}