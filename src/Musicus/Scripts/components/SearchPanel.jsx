import React, { PropTypes } from 'react'
import { connect } from 'react-redux';
import SearchFilter from './SearchFilter.jsx'
import SearchResult from './SearchResult.jsx'
import SearchBar from './SearchBar.jsx'
import SearchAlbumResult from './SearchAlbumResult.jsx'
import Ink from 'react-ink';
import { setSpotifyFilter, search, addToQueue } from '../actions/generalActions.jsx'
import * as types from '../constants/Types.jsx'
import $ from 'jquery'

class SearchPanel extends React.Component {
    constructor(props) {
        super(props);
        this.onSearchFilterClick = this.onSearchFilterClick.bind(this);
        this.search = this.search.bind(this);
        this.addToQueue = this.addToQueue.bind(this);
        this.onEnterKeyPress = this.onEnterKeyPress.bind(this);
    }

    onSearchFilterClick(type) {
        switch (type) {
            case types.SPOTIFY:
                this.props.setSpotifyFilter();
                break;
        }
    }

    search() {
        const searchKey = $('#tbSearch').val();
        this.props.search(searchKey);
    }

    addToQueue(trackId, artist, description, trackLength, url, source) {        
        this.props.addToQueue(trackId, artist, description, trackLength, url, source);
    }

    onEnterKeyPress(e) {
        if(e.key === 'Enter'){
            const searchKey = $('#tbSearch').val();
            this.props.search(searchKey); 
        }
    }
    render() {
        return (
            <div className="search-panel">
                <SearchBar />
                {/* <SearchAlbumResult /> */}
                <div className="search-result-list">
                {
								this.props.searchResult.map(line =>
									<SearchResult
										artist={line.artist}
										description={line.description}
										searchtype={line.type}
										trackid={line.trackId}
										tracklength={line.trackLength}
										trackcount={line.trackCount}
										url={line.url}
										tracksource={line.trackSource}
										onclick={this.addToQueue} />
                    )
                }
                </div>
            </div>)
    }
}

SearchPanel.propTypes = {
}

function mapStateToProps(state) {
    return {
			searchResult: state.musicusState.searchResult
    }
}

export default connect(mapStateToProps, { setSpotifyFilter, search, addToQueue })(SearchPanel)
