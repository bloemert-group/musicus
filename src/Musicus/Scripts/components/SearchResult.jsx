import React from 'react'
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Ink from 'react-ink';
import * as types from '../constants/Types.jsx'
import moment from 'moment'

class SearchResult extends React.Component {
	constructor(props) {
		super(props);
		this.click = this.click.bind(this);

	}

	click(e) {
		e.preventDefault();
		this.props.onclick(this.props.trackid, this.props.artist, this.props.description, this.props.tracklength, this.props.url, this.props.tracksource);
	}

	render() {

		var momentLength = moment.duration(this.props.tracklength);

		var trackLengthFormatted = `${momentLength.minutes()}:${momentLength.seconds().pad()}`;

		var playlistTracks = `Tracks: ${this.props.trackcount}`;

		var durationTrackCount = (<div className="search-list-item-duration">
			{trackLengthFormatted}
		</div>);
		if (this.props.searchtype === 1) {
			var durationTrackCount = (<div className="search-list-item-tracks">
				{playlistTracks}
			</div>);
		}

		var lineDescription = this.props.artist;
		if (lineDescription === null || lineDescription === "") {
			lineDescription = this.props.description;
		}
		else if (this.props.description !== null && this.props.description !== "") {
			lineDescription += ' - ' + this.props.description;
		}

		return (
      <div className="search-list-item" onClick={this.click}>
        <div className="search-list-item-tracksource">
          <i className={this.props.icon}></i>
        </div>
				<div className="search-list-item-title">
					{lineDescription}
				</div>
				{durationTrackCount}
				<div className="search-list-item-add">
					<i className="plus icon"></i>
        </div>
				<Ink />
			</div>)
	}
}

SearchResult.propTypes = {
	trackid: PropTypes.string,
	artist: PropTypes.string,
	description: PropTypes.string,
	onclick: PropTypes.func,
	tracklength: PropTypes.number,
	searchtype: PropTypes.number,
	trackcount: PropTypes.number,
	url: PropTypes.string,
  tracksource: PropTypes.string,
  icon: PropTypes.string
}

function mapStateToProps(state) {
  return {    
	}
}

export default connect(mapStateToProps)(SearchResult)
