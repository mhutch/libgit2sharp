﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LibGit2Sharp
{
    /// <summary>
    /// Provides helper overloads to a <see cref="Network"/>.
    /// </summary>
    public static class NetworkExtensions
    {
        /// <summary>
        /// Push the specified branch to its tracked branch on the remote.
        /// </summary>
        /// <param name="network">The <see cref="Network"/> being worked with.</param>
        /// <param name="branch">The branch to push.</param>
        /// <exception cref="LibGit2SharpException">Throws if either the Remote or the UpstreamBranchCanonicalName is not set.</exception>
        public static void Push(
            this Network network,
            Branch branch)
        {
            network.Push(new[] { branch });
        }
        /// <summary>
        /// Push the specified branch to its tracked branch on the remote.
        /// </summary>
        /// <param name="network">The <see cref="Network"/> being worked with.</param>
        /// <param name="branch">The branch to push.</param>
        /// <param name="pushOptions"><see cref="PushOptions"/> controlling push behavior</param>
        /// <exception cref="LibGit2SharpException">Throws if either the Remote or the UpstreamBranchCanonicalName is not set.</exception>
        public static void Push(
            this Network network,
            Branch branch,
            PushOptions pushOptions)
        {
            network.Push(new[] { branch }, pushOptions);
        }

        /// <summary>
        /// Push the specified branches to their tracked branches on the remote.
        /// </summary>
        /// <param name="network">The <see cref="Network"/> being worked with.</param>
        /// <param name="branches">The branches to push.</param>
        /// <exception cref="LibGit2SharpException">Throws if either the Remote or the UpstreamBranchCanonicalName is not set.</exception>
        public static void Push(
            this Network network,
            IEnumerable<Branch> branches)
        {
            network.Push(branches, null);
        }

        /// <summary>
        /// Push the specified branches to their tracked branches on the remote.
        /// </summary>
        /// <param name="network">The <see cref="Network"/> being worked with.</param>
        /// <param name="branches">The branches to push.</param>
        /// <param name="pushOptions"><see cref="PushOptions"/> controlling push behavior</param>
        /// <exception cref="LibGit2SharpException">Throws if either the Remote or the UpstreamBranchCanonicalName is not set.</exception>
        public static void Push(
            this Network network,
            IEnumerable<Branch> branches,
            PushOptions pushOptions)
        {
            var enumeratedBranches = branches as IList<Branch> ?? branches.ToList();

            foreach (var branch in enumeratedBranches)
            {
                if (string.IsNullOrEmpty(branch.UpstreamBranchCanonicalName))
                {
                    throw new LibGit2SharpException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "The branch '{0}' (\"{1}\") that you are trying to push does not track an upstream branch.",
                            branch.FriendlyName, branch.CanonicalName));
                }
            }

            foreach (var branch in enumeratedBranches)
            {
                network.Push(branch.Remote, string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}:{1}", branch.CanonicalName, branch.UpstreamBranchCanonicalName), pushOptions);
            }
        }
    }
}
