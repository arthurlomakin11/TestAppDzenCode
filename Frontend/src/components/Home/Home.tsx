import React from 'react';
import HomeStyles from "./Home.module.scss";
import {Comment} from "./Comment";
import {CommentsHeader} from "./CommentsHeader";

export let Home = () =>  {
   return (
      <div>
        <div className={HomeStyles.CommentsSection}>
          <CommentsHeader/>
          
          <ul className={HomeStyles.ContentListComments}>
            <li className={HomeStyles.ContentListItem}>
              <Comment>
                  <ul className={HomeStyles.ContentListNestedComments}>
                      <li className={HomeStyles.ContentListItem}>
                          <Comment>
                              <></>
                          </Comment>
                      </li>
                  </ul>
              </Comment>
            </li>
          </ul>
        </div>
      </div>
    );
}