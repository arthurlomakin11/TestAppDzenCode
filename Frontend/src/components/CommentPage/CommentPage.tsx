﻿import React, {useEffect, useState} from 'react';
import HomeStyles from "./Home.module.scss";
import {Comment} from "../Comment/Comment";
import {CommentsHeader} from "../Comment/CommentsHeader";
import {IComment} from "../../interfaces/IComment";

export let CommentPage = () =>  {
    const [state, setState] = useState({
        comments: [] as IComment[],
        loading: true
    })

    useEffect(() => {
        (async () => {
            const response = await fetch("api/comments");
            const data:IComment[] = await response.json();
            setState({comments: data, loading: false});
        })()
    }, [])

    return state.loading
        ? <p><em>Loading...</em></p>
        : <div>
            <div className={HomeStyles.CommentsSection}>
                <CommentsHeader/>

                <ul className={HomeStyles.ContentListComments}>
                    {
                        state.comments.map(comment => {
                            return <li className={HomeStyles.ContentListItem} key={comment.Id}>
                                <Comment comment={comment}/>
                            </li>
                        })
                    }
                </ul>
            </div>
        </div>;
}