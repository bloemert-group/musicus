import React from 'react'
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Ink from 'react-ink';
import * as types from '../constants/Types.jsx'

class SearchFilter extends React.Component {
  constructor(props) {
    super(props);
    this.click = this.click.bind(this);
  }

  click(e) {    
    e.preventDefault();
    this.props.onclick(this.props.type);
  }

  render() {
    let style = { background: 'none' }

    switch (this.props.type) {
      case types.SPOTIFY:
        if (this.props.spotify) {
          style = { background: 'blue' }
        }
        break;
    }

    return (
      <button onTouchTap={this.click} className="searchfilter" style={style}>
        {this.props.name}
        <Ink />
      </button>)
  }
}

SearchFilter.propTypes = {
  name: PropTypes.string,
  type: PropTypes.number,
  onclick: PropTypes.func
}

function mapStateToProps(state) {
  return {
		spotify: state.musicusState.searchFilter.spotify
  }
}

export default connect(mapStateToProps)(SearchFilter)
